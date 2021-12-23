using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIDev.lib.engines
{
  internal class MCHEngine
  {

    public List<Action> getPlan( /* TODO gameState */ )
    {
      //  TODO
      //    Compute additional information
      //    For now, the plain opener is used for mocking
      return this.getOpener();
    }

    private List<Action> getOpener()
    {
      List<Action> actions = new List<Action>();
      actions.Add(new Action() { ActionName = "Reassemble", ActionType = ActionTypes.oGCD });
      actions.Add(new Action() { ActionName = "Air Anchor", ActionType = ActionTypes.GCD });
      actions.Add(new Action() { ActionName = "Guass Round", ActionType = ActionTypes.oGCD });
      actions.Add(new Action() { ActionName = "Ricochet", ActionType = ActionTypes.oGCD });
      actions.Add(new Action() { ActionName = "Drill", ActionType = ActionTypes.GCD });

      //  There's more here I promise

      return actions;
    }
  }
}
