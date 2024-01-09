using Charts.ServicesAbstract;

namespace Charts.Interfaces
{
    public interface ITransmiter
    {
        public event EventHandler<TransmiteDateArgs> SendingDate;
    }
}
