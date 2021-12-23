using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImGuiNET;
using ImGuiScene;

namespace UIDev.lib
{
  internal class DisplayEngine
  {
    public void draw(List<Action> plan)
    {
      ImGui.Text("Hello World!");

      foreach (Action action in plan)
      {
        ImGui.Text(action.ActionName);
      }
    }
  }
}
