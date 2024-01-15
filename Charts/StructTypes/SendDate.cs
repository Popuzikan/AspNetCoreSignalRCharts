namespace Charts.StructTypes
{
    public struct SendDate
    {
        public UInt16 StructHeader;
        public Int16 Azimut_flag;
        public Int16 Azimut;
        public Int16 UgolMesta_flag;
        public UInt16 UgolMesta;

        public bool[] isPmActive;

        public SendDate(DataDevice dataDevice)
        {
            isPmActive = new bool[dataDevice.PmStack.Length];
            StructHeader = 0xAA11;
            Azimut_flag = 1;
            Azimut = 0;
            UgolMesta_flag = 0;
            UgolMesta = 0;

            for (int i = 0; i < isPmActive.Length; i++)
            {
                isPmActive[i] = bool.Parse(dataDevice.PmStack[i]);
            }        
        }
    }
}
