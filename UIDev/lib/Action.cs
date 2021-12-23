using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIDev.lib
{
  public class Action : IEquatable<Action>
  {

    public string? ActionName { get; set; }
    public ActionTypes? ActionType { get; set; }

    public override string ToString()
    {
      return $"{this.ActionName}, Type: {this.ActionType}";
    }

    public override int GetHashCode()
    {
      if (this.ActionType != null) { return this.ActionType.GetHashCode(); }
      return 0;
    }

    public bool Equals(Action? other)
    {
      if (other == null) { return false;}
      if (ReferenceEquals(this, other)) { return true;}
      if (other.ActionName == this.ActionName) { return true;}
      return false;
    }
  }
}
