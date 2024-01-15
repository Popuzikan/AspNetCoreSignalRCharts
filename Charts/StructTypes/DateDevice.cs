namespace Charts.StructTypes
{
    public struct DataDevice
    {
        //public UInt16 StructHeader;
        //public Int16 Azimut_flag;
        //public Int16 Azimut;
        //public Int16 UgolMesta_flag;
        //public UInt16 UgolMesta;

        public string[] PmStack { get; set; } // массив строкового представления для включения RF

        //public bool[] isPmActive;
        //public bool _pm2;
        //public bool _pm3;
        //public bool _pm4;

        public DataDevice(string[] pmStack)
        {
            //isPmActive = new bool[pmStack.Length];
            //StructHeader = 0xAA11;
            //Azimut_flag = 1;
            //Azimut = 0;
            //UgolMesta_flag = 0;
            //UgolMesta = 0;
            PmStack = pmStack;

            //for (int i = 0; i < pmStack.Length; i++)
            //{
            //    isPmActive[i] = bool.Parse(pmStack[i]);
            //}
            ////_pm2 = pm2;
            //_pm3 = pm3;
            //_pm4 = pm4;
        }

        /// <summary>
        /// false - включить Jammer
        /// true - отключить Jammer
        /// </summary>
        /// 
    }



}
