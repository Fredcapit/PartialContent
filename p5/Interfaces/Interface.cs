using System;
using System.Threading.Tasks;

namespace p5
{
    public interface PartialContentProducerInerface
    {

        Task isAvailable();
        Task IsRangeHeaderExist();
        Task CanRangeBeFulfilled();
        Task ProduceConent();
    }
}
