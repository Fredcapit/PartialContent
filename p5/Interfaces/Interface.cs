using System;
using System.Threading.Tasks;

namespace p5.interfaces
{
    public interface PartialContentProducerInerface
    {

        bool isAvailable();
        bool CanRangeBeFulfilled();
        void ProduceConent();
    }
}
