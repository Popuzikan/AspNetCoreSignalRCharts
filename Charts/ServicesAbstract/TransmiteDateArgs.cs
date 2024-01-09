using Charts.Services;

namespace Charts.ServicesAbstract
{
    public class TransmiteDateArgs : EventArgs
    {
        public PointRF Samples { get; private set; }

        public TransmiteDateArgs(PointRF point)
        {              
            Samples = point;
        }
    }
}
