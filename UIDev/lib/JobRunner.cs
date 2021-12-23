using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIDev.lib.engines;

namespace UIDev.lib
{
  internal class JobRunner
  {
    public List<Action> getPlan(Jobs job)
    {
      switch (job) {
        case Jobs.MCH:
          return new MCHEngine().getPlan();
        default:
          //  Return Empty List
          return new List<Action>();
      }
    }
  }
}
