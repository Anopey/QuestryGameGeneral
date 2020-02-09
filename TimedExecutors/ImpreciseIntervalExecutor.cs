using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace QuestryGameGeneral.TimedExecutors
{
    //TODO: IMPLEMENT PRECISE ONE TOO

    /// <summary>
    /// Executes a given function for every specified interval, unless the time given for one of the functions is not perfectly divided in which scenario the 
    /// function will be once executed with the remainder. Especially useful for time-sensitive functions. The difference between the precise executor type
    /// is that this one will not be delayed in starting, meaning that the first execution will happen much faster than the ideal execution interval time, and that
    /// it is no considered erronous for the executions to happen faster than the ideal time (of course the proper time passed will be input)
    /// </summary>
    public class ImpreciseIntervalExecutor
    {

        #region Types

        /// <summary>
        /// The type used for the Assigner and Disconnectors.
        /// </summary>
        public delegate void Operation();

        /// <summary>
        /// The type used for Exeuction.
        /// </summary>
        /// <param name="time"> Time in milliseconds that has passed since last execution. </param>
        public delegate void Execution(long time);

        /// <summary>
        /// The optional type called for when the kernel calls the thread of this object too late for the execution to be called within margin. This function will
        /// be called before the execution takes place.
        /// </summary>
        /// <param name="assigner"> The assigner function used </param>
        /// <param name="disconnector"> The disconnector function used </param>
        /// <param name="executor"> The executor function used. </param>
        /// <param name="time_left"> The time left to be executed before all the intervals of execution were finished. This does not include the current interval.
        /// </param>
        /// <param name="current_interval"> The current interval of time before execution was stopped. This does not include the time elapsed after this function 
        /// is called. </param>
        /// <returns> Whether the thread should continue as normal to execute. </returns>
        public delegate bool ErrorHandler(Operation assigner, Operation disconnector, Execution executor, long time_left, long current_interval);

        class ExecutionCollection
        {
            public Operation Assigner;
            public Operation Disconnector;
            public Execution Executor;

            public ExecutionCollection(Operation assigner, Operation disconnector, Execution executor)
            {
                Assigner = assigner;
                Disconnector = disconnector;
                Executor = executor;
            }
        }

        class QueuedCollection
        {
            public Stopwatch StopWatch;
            public long ExecutionTime;
            public ExecutionCollection execollec;

            public QueuedCollection(Stopwatch sw, long exetime, ExecutionCollection executionCollection)
            {
                StopWatch = sw;
                ExecutionTime = exetime;
                execollec = executionCollection;
            }
        }

        private bool DefaultHandleError(Operation assigner, Operation disconnector, Execution executor, long time_left, long current_interval)
        {
            return true;
        }

        #endregion

        private Stack<int> CustomIntervalTimes;
        private Dictionary<int, ExecutionCollection> Executions;
        private Dictionary<int, QueuedCollection> Queued;
        private Dictionary<ExecutionCollection, long> Entries;
        private Dictionary<ExecutionCollection, int> Delays;
        private int IdealInterval;
        private int ErrorMargin;
        private int CurrentInterval;
        private Stopwatch Timer;
        private ErrorHandler HandleError;
        private bool ExecutionTimeIncluded;
        private ThreadPriority TPriority;
        private Thread ExecutionThreadInstance;
        private bool ResetQueued;

        private bool ExecutionThreadActive = false;
        private bool StartThreadIfAdded = false;

        private int MinID = 0;
        private Queue<int> VacantIDs;

        #region Constructors

        /// <summary>
        /// Instantiates an Interval Executor. 
        /// </summary>
        /// <param name="interval"> The interval of time in milliseconds that is waited before the executions will be executed (ideally). </param>     
        /// <param name="execution_time_included"> Whether the execution time of the execution is to be included in the interval waited between executions. </param>
        /// <param name="tpriority"> The priority at which the thread enumerating through the entries should be set. </param>
        /// <param name="reset_queued"> True for when the timers should only be started when the whole object is started through StartExecution(), 
        /// or false for immediately when the execution is added through AddEntry(). The associator, if defined, will also be called right before when the timer starts. </param>
        public ImpreciseIntervalExecutor(int interval, bool execution_time_included, ThreadPriority tpriority, bool reset_queued)
        {
            Queued = new Dictionary<int, QueuedCollection>();
            Entries = new Dictionary<ExecutionCollection, long>();
            IdealInterval = interval;
            ErrorMargin = -1;
            CurrentInterval = IdealInterval;
            Timer = Stopwatch.StartNew();
            Timer.Stop();
            HandleError = DefaultHandleError;
            ExecutionTimeIncluded = execution_time_included;
            CustomIntervalTimes = new Stack<int>();
            Delays = new Dictionary<ExecutionCollection, int>(); //TODO Make sure that, whenever a new entry is added, it is added to this with int = 0.
            TPriority = tpriority;
            Executions = new Dictionary<int, ExecutionCollection>();
            VacantIDs = new Queue<int>();
            ResetQueued = reset_queued;
        }

        /// <summary>
        /// Instantiates an Interval Executor. 
        /// </summary>
        /// <param name="interval"> The interval of time in milliseconds the executions will be executed (ideally). </param>
        /// <param name="margin"> The margin of error as to how late the methods may be executed. </param>
        /// <param name="handler"> A custom function to be executed when the error is above the margin. </param>            
        /// <param name="execution_time_included"> Whether the execution time of the execution is to be included in the interval waited between executions. </param>
        /// <param name="tpriority"> The priority at which the thread enumerating through the entries should be set. </param>
        /// <param name="reset_queued"> True for when the timers should only be started when the whole object is started through StartExecution(), 
        /// or false for immediately when the execution is added through AddEntry(). The associator, if defined, will also be called right before when the timer starts. </param>
        public ImpreciseIntervalExecutor(int interval, bool execution_time_included, ThreadPriority tpriority, bool reset_queued, int margin, ErrorHandler handler)
        {
            Queued = new Dictionary<int, QueuedCollection>();
            Entries = new Dictionary<ExecutionCollection, long>();
            IdealInterval = interval;
            ErrorMargin = margin;
            CurrentInterval = IdealInterval;
            Timer = Stopwatch.StartNew();
            Timer.Stop();
            HandleError = handler;
            ExecutionTimeIncluded = execution_time_included;
            CustomIntervalTimes = new Stack<int>();
            Delays = new Dictionary<ExecutionCollection, int>(); //TODO Make sure that, whenever a new entry is added, it is added to this with int = 0.
            TPriority = tpriority;
            Executions = new Dictionary<int, ExecutionCollection>();
            VacantIDs = new Queue<int>();
            ResetQueued = reset_queued;
        }

        #endregion

        #region Execution

        private int[] ExecutionOrder = new int[0];
        private bool ExecutedForward = true;
        private bool CancelCurrentDisAssociation = false;

        private void ExecutionThread()
        {
            Timer.Reset();
            Timer.Start();
            Stopwatch adjuster = Stopwatch.StartNew();
            adjuster.Stop();
            Thread.CurrentThread.Priority = TPriority;
            while (ExecutionThreadActive)
            {
                bool error = false;
                lock (ExecutionOrder)
                {
                    if (ExecutedForward)
                    {
                        ExecutedForward = false;
                        ExecutionOrder = Executions.Keys.ToArray();
                    }
                    else
                    {
                        ExecutedForward = true;
                        int[] temp = Executions.Keys.ToArray();
                        ExecutionOrder = new int[temp.Length];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            ExecutionOrder[i] = temp[ExecutionOrder.Length - 1 - i];
                        }
                    }
                }
                if ((CurrentInterval - (int)Timer.ElapsedMilliseconds) - (int)adjuster.ElapsedMilliseconds > 0)
                    Thread.Sleep((CurrentInterval - (int)Timer.ElapsedMilliseconds - (int)adjuster.ElapsedMilliseconds));
                else
                {
                    if (ErrorMargin >= 0 && Timer.ElapsedMilliseconds > IdealInterval + ErrorMargin)
                        error = true;
                    CurrentInterval = (int)Timer.ElapsedMilliseconds;
                }
                if (!ExecutionThreadActive) return;
                lock (Entries) //TODO Optimize this for multithreading. If several cores are present, run multiple executions at once. MAybe can use ThreadPool just for lines before an exeuction? That way this thread will keep running but executions may nonetheless occur. Make sure to provide critical info that the user may instaed try to fetch from the class and make sure the same operation does not get called twice.
                {
                    if (!ExecutionTimeIncluded)
                        Timer.Stop();
                    int min = CustomIntervalTimes.Count == 0 ? IdealInterval : CustomIntervalTimes.Peek();
                    long time;
                    List<int> ToBeRemoved = new List<int>();
                    adjuster.Reset();
                    adjuster.Start();
                    foreach (int i in ExecutionOrder)
                    {
                        ExecutionCollection exec = Executions[i];
                        time = Timer.ElapsedMilliseconds;
                        if ((ErrorMargin >= 0 && time + Delays[exec] > IdealInterval + ErrorMargin))
                        {
                            //error!
                            if (!HandleError(exec.Assigner, exec.Disconnector, exec.Executor, Entries[exec], time + Delays[exec]))
                                continue;
                        }
                        if (error)
                        {
                            //thread time related error.
                            if (!HandleError(exec.Assigner, exec.Disconnector, exec.Executor, Entries[exec], time + Delays[exec]))
                                continue;
                        }
                        time = Timer.ElapsedMilliseconds;
                        Entries[exec] -= time + Delays[exec];
                        exec.Executor(time + Delays[exec]);
                        Delays[exec] = (int)Timer.ElapsedMilliseconds;

                        if (Entries[exec] < min)
                        {
                            if (Entries[exec] > 0)
                            {
                                //this entry is going to run out of time before interval is finished. Adjust interval accordingly.
                                CustomIntervalTimes.Push((int)Entries[exec]);
                                min = (int)Entries[exec];
                            }
                            else
                            {
                                //this entry has already run out of time.
                                exec.Disconnector();
                                if (CancelCurrentDisAssociation)
                                {
                                    Entries[exec] = 1000000;
                                    CancelCurrentDisAssociation = false;
                                    continue;
                                }
                                //TODO Actually add remover function to also check for when this thread should be stopped.
                                Entries.Remove(exec);
                                Delays.Remove(exec);
                                ToBeRemoved.Add(i);
                            }
                        }
                    }


                    foreach (int i in ToBeRemoved)
                    {
                        Executions.Remove(i);
                        VacantIDs.Enqueue(i);
                    }

                    if (CustomIntervalTimes.Count > 0)
                        CurrentInterval = CustomIntervalTimes.Pop();
                    else
                        CurrentInterval = IdealInterval;

                    foreach (int i in Executions.Keys)
                    {
                        Delays[Executions[i]] = (int)Timer.ElapsedMilliseconds - Delays[Executions[i]];
                    }

                }

                adjuster.Stop();
                Timer.Reset();
                Timer.Start();

                lock (Queued)
                {

                    long effectivetime;
                    int min = CurrentInterval;
                    foreach (int i in Queued.Keys)
                    {
                        effectivetime = Queued[i].StopWatch.ElapsedMilliseconds - Timer.ElapsedMilliseconds;
                        Queued[i].ExecutionTime -= effectivetime;
                        Queued[i].execollec.Executor(effectivetime);
                        Executions.Add(i, Queued[i].execollec);
                        Entries.Add(Queued[i].execollec, Queued[i].ExecutionTime);
                        Delays.Add(Queued[i].execollec, 0);
                        if (min > Queued[i].ExecutionTime)
                        {
                            min = (int)Queued[i].ExecutionTime;
                            CustomIntervalTimes.Push(CurrentInterval);
                            CurrentInterval = min;
                        }
                    }
                    Queued.Clear();
                }

                if (Entries.Count == 0 && Queued.Count == 0)
                    ExecutionThreadActive = false; //stop thread if there is nothing to execute.

            }
        }

        /// <summary>
        /// Starts the execution of the entries.
        /// </summary>
        public void StartExecution()
        {
            Stopwatch ree = Stopwatch.StartNew();
            ExecutionThreadInstance = new Thread(ExecutionThread);
            if (!StartThreadIfAdded)
            {
                StartThreadIfAdded = true;
                if (!ResetQueued)
                {
                    int total = 0;
                    Stopwatch sw = Stopwatch.StartNew();
                    int count = Queued.Count;
                    foreach (QueuedCollection que in Queued.Values)
                    {
                        total += (int)que.StopWatch.ElapsedMilliseconds;
                    }
                    if (total > IdealInterval)
                        CurrentInterval = 0;
                    else
                        CurrentInterval = IdealInterval - ((total / count) + (1 / 2) * (int)sw.ElapsedMilliseconds);
                    ExecutionThreadInstance.Start();
                    sw.Stop();
                }
                else
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    foreach (QueuedCollection que in Queued.Values)
                    {
                        que.StopWatch.Reset();
                        que.execollec.Assigner();
                        que.StopWatch.Start();
                    }
                    CurrentInterval = IdealInterval - (1 / 2) * (int)sw.ElapsedMilliseconds;
                    ExecutionThreadInstance.Start();
                    sw.Stop();
                }
                return;
            }
            ExecutionThreadActive = true;
            CurrentInterval = IdealInterval - (int)ree.ElapsedMilliseconds;
            ExecutionThreadInstance.Start();
        }

        /// <summary>
        /// Stops the execution of the entries. If in the middle of an execution, waits for it to end.
        /// </summary>
        /// <param name="ImmediateSuspend"> UNSAFE. Ends the execution thread(s) immediately. This might corrupt data or lead to unpredictable behaviour. 
        /// Use at your own risk. </param>
        public void StopExecution(bool ImmediateSuspend = false)
        {
            StartThreadIfAdded = false;
            ExecutionThreadActive = false;
            if (ImmediateSuspend)
                ExecutionThreadInstance.Abort();
        }

        #endregion

        #region Entry Manager

        #region Defaults

        private void DefaultAssigner()
        {

        }

        private void DefaultDisAssociator()
        {

        }

        private void DefaultInfiniteDisAssociator()
        {
            CancelCurrentDisAssociation = true;
        }

        #endregion

        /// <summary>
        /// Adds an execution to be run in specified intervals.
        /// </summary>
        /// <param name="exec"> The execution. Must take integer value that represents the amount of time that has passed. </param>
        /// <param name="execution_time"> The amount of total time it is to be executed before it is released. negative if you wish it to never be released. </param>
        /// <returns> The unique ID representing this execution. Store it if you would like to access the execution later on. </returns>
        public int AddEntry(Execution exec, long execution_time)
        {
            Stopwatch sw = Stopwatch.StartNew();
            int i = GetUniqueID();
            Operation disassociator = DefaultDisAssociator;
            if (execution_time < 0)
            {
                execution_time = 100000;
                disassociator = DefaultInfiniteDisAssociator;
            }
            QueuedCollection collec = new QueuedCollection(sw, execution_time, new ExecutionCollection(DefaultAssigner, disassociator, exec));
            Queued.Add(i, collec);
            if (StartThreadIfAdded && !ExecutionThreadActive)
                StartExecution();
            return i;
        }


        /// <summary>
        /// Adds an execution to be run in specified intervals.
        /// </summary>
        /// <param name="exec"> The execution. Must take integer value that represents the amount of time that has passed. </param>
        /// <param name="disassociator"> The disassociator. Executed once when the execution's last execution is executed. </param>
        /// <param name="execution_time"> The amount of total time it is to be executed before it is released. negative if you wish it to never be released. This will
        /// override any disassociator input. </param>
        /// <returns> The unique ID representing this execution. Store it if you would like to access the execution later on. </returns>
        public int AddEntry(Execution exec, Operation disassociator, long execution_time)
        {
            Stopwatch sw = Stopwatch.StartNew();
            int i = GetUniqueID();
            if (execution_time < 0)
            {
                execution_time = 1000000;
                disassociator = DefaultInfiniteDisAssociator;
            }
            QueuedCollection collec = new QueuedCollection(sw, execution_time, new ExecutionCollection(DefaultAssigner, disassociator, exec));
            Queued.Add(i, collec);
            if (StartThreadIfAdded && !ExecutionThreadActive)
                StartExecution();
            return i;
        }


        /// <summary>
        /// Adds an execution to be run in specified intervals.
        /// </summary>
        /// <param name="associator"> The associator. Run in accordance with the ResetQueued Property set when this object was constructed. 
        /// The associators of a single object should not differ too much in workload, as otherwise there might be some large deviations in the start
        /// times of the different executions.</param>
        /// <param name="exec"> The execution. Must take integer value that represents the amount of time that has passed. </param>
        /// <param name="disassociator"> The disassociator. Executed once when the execution's last execution is executed. </param>
        /// <param name="execution_time"> The amount of total time it is to be executed before it is released. negative if you wish it to never be released. This will
        /// override any disassociator input. </param>
        /// <returns> The unique ID representing this execution. Store it if you would like to access the execution later on. </returns>
        public int AddEntry(Operation associator, Execution exec, Operation disassociator, long execution_time)
        {
            if (!ResetQueued)
            {
                associator();
            }
            Stopwatch sw = Stopwatch.StartNew();
            int i = GetUniqueID();
            if (execution_time < 0)
            {
                execution_time = 1000000;
                disassociator = DefaultInfiniteDisAssociator;
            }
            QueuedCollection collec = new QueuedCollection(sw, execution_time, new ExecutionCollection(associator, disassociator, exec));
            Queued.Add(i, collec);
            if (StartThreadIfAdded && !ExecutionThreadActive)
                StartExecution();
            return i;
        }

        /// <summary>
        /// Removes an execution entry based on its ID. If an entry is about to be run the entry may not be removed first.
        /// </summary>
        /// <param name="EntryID"> ID of execution to be removed. </param>
        /// <param name="CallDisAssociator"> Whether the disassociator function should be called. ıf none assigned at the start, this is meaningless. </param>
        /// <returns> Whether the entry was existant or not. </returns>
        public bool RemoveEntry(int EntryID, bool CallDisAssociator)
        {
            lock (Entries)
            {
                lock (Queued)
                {
                    if (Executions.Keys.Contains(EntryID))
                    {
                        //is in executions.

                        //check if still there:
                        if (Executions.Keys.Contains(EntryID))
                        {
                            lock (ExecutionOrder)
                            {
                                //remove from execution order.
                                if (ExecutionOrder.Contains(EntryID))
                                {
                                    for (int i = 0; i < ExecutionOrder.Length; i++)
                                    {
                                        if (ExecutionOrder[i] == EntryID)
                                        {
                                            for (int j = i; j < ExecutionOrder.Length - 1; j++)
                                            {
                                                ExecutionOrder[j] = ExecutionOrder[j + 1];
                                            }
                                            Array.Resize<int>(ref ExecutionOrder, ExecutionOrder.Length - 1);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (CallDisAssociator)
                            Executions[EntryID].Disconnector();
                        Entries.Remove(Executions[EntryID]);
                        Delays.Remove(Executions[EntryID]);
                        Executions.Remove(EntryID);
                        VacantIDs.Enqueue(EntryID);

                        return true;
                    }
                    else if (Queued.Keys.Contains(EntryID))
                    {
                        //is in queue.
                        if (CallDisAssociator)
                            Queued[EntryID].execollec.Disconnector();
                        Queued.Remove(EntryID);

                        return true;
                    }
                }
            }
            return false;
        }


        #endregion

        #region General Commands



        #endregion

        #region Getters

        /// <summary>
        /// Returns the total milliseconds of time the specified execution has before it is removed. Meaningless 
        /// for indefinite executions, as these get immediately revived after they "run out" of time.
        /// </summary>
        /// <param name="ExecutionID"> The Unique ID that identifies this execution. </param>
        /// <returns> The total milliseconds of time the specified execution has before it is removed. returns long.MinValue if wrong ID is supplied </returns>
        public long GetExecutionTotalTimeLeft(int ExecutionID)
        {
            try
            {
                if (Executions.ContainsKey(ExecutionID))
                    return Entries[Executions[ExecutionID]];
                return Queued[ExecutionID].ExecutionTime;
            }
            catch
            {
                return long.MinValue;
            }
        }

        #endregion

        #region ID

        private int GetUniqueID()
        {
            if (VacantIDs.Count == 0)
                return MinID++;
            return VacantIDs.Dequeue();
        }

        #endregion

    }
}
