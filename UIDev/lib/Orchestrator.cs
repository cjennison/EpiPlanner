using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;

namespace UIDev.lib
{
  public class Orchestrator
  {
    private ListenerMock listener;
    //    Real listener would have identical systems
    //  private Listener listener;

    private DisplayEngine displayEngine;
    private JobRunner jobRunner;

    public Orchestrator()
    {
      this.listener = new ListenerMock();
      //  this.listener = new Listener();
      this.displayEngine = new DisplayEngine();
      this.jobRunner = new JobRunner();
    }

    public void run()
    {
      Trace.WriteLine("Orchestrator run()");

      this.displayEngine.draw();
    }
  }
}
