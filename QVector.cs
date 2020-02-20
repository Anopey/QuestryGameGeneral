using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral
{
    
    //TODO DECLARED INTERNAL WHILE IT IS BEING WORKED ON.
    internal class QVector
    {
        private int Dimensions;

        private float[] Values;

        public int GetDimensions()
        {
            return Dimensions;
        }

        public QVector(int dimensions)
        {
            Dimensions = dimensions;
            Values = new float[dimensions];
        }



    }
}
