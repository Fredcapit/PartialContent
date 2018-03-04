using System;
using System.Threading.Tasks;

namespace p5.interfaces
{
    public interface PartialContentProducerInerface
    {

        Task<bool> isAvailable();
        Task IsRangeHeaderExist();
        Task CanRangeBeFulfilled();
        Task ProduceConent();
    }
}
