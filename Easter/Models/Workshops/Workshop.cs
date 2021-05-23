using Easter.Models.Bunnies.Contracts;
using Easter.Models.Eggs.Contracts;
using Easter.Models.Workshops.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easter.Models.Workshops
{
    public class Workshop : IWorkshop
    {
        public Workshop()
        {

        }
        public void Color(IEgg egg, IBunny bunny)
        {
           
                foreach (var dye in bunny.Dyes)
                {
                    if (bunny.Energy == 0)
                    {
                        break;
                    }

                    if (dye.IsFinished())
                    {
                        continue;
                    }
                    if (egg.IsDone())
                    {
                        break;
                    }
                    while (true)
                    {
                        if (bunny.Energy == 0)
                        {
                            break;
                        }
                        bunny.Work();
                        dye.Use();
                        egg.GetColored();
                        if (egg.IsDone())
                        {
                            break;
                        }
                        if (dye.IsFinished())
                        {
                            break;
                        }

                    }
                }
           
                        
        }
    }
}
