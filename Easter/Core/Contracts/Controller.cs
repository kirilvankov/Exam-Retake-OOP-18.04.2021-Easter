using Easter.Models.Bunnies;
using Easter.Models.Bunnies.Contracts;
using Easter.Models.Dyes;
using Easter.Models.Dyes.Contracts;
using Easter.Models.Eggs;
using Easter.Models.Eggs.Contracts;
using Easter.Models.Workshops;
using Easter.Repositories;
using Easter.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easter.Core.Contracts
{
    public class Controller : IController
    {
        private BunnyRepository bunnyRepository;
        private EggRepository eggRepository;
        private Workshop workshop;
        private int coloredEggs = 0;
        public Controller()
        {
            this.bunnyRepository = new BunnyRepository();
            this.eggRepository = new EggRepository();
            this.workshop = new Workshop();
        }
        public string AddBunny(string bunnyType, string bunnyName)
        {
            IBunny bunny = null;
            if (bunnyType == "HappyBunny")
            {
                bunny = new HappyBunny(bunnyName);
            }
            else if (bunnyType == "SleepyBunny")
            {
                bunny = new SleepyBunny(bunnyName);
            }
            else
            {
                throw new ArgumentException(ExceptionMessages.InvalidBunnyType);
            }
            this.bunnyRepository.Add(bunny);
            return string.Format(OutputMessages.BunnyAdded, bunny.GetType().Name, bunny.Name);
        }

        public string AddDyeToBunny(string bunnyName, int power)
        {
            IBunny bunny = this.bunnyRepository.FindByName(bunnyName);
            if (bunny == null)
            {
                throw new InvalidOperationException(ExceptionMessages.InexistentBunny);
            }
            Dye dye = new Dye(power);
            bunny.AddDye(dye);
            return string.Format(OutputMessages.DyeAdded, dye.Power, bunny.Name);

        }

        public string AddEgg(string eggName, int energyRequired)
        {
            Egg egg = new Egg(eggName, energyRequired);
            this.eggRepository.Add(egg);
            return string.Format(OutputMessages.EggAdded, egg.Name);
        }

        public string ColorEgg(string eggName)
        {
            IEgg egg = this.eggRepository.FindByName(eggName);
            
            bool isExistReady = false;

            foreach (var bunny in this.bunnyRepository.Models.OrderByDescending(b => b.Energy))
            {
                if (bunny.Energy >= 50)
                {
                    isExistReady = true;
                    this.workshop.Color(egg, bunny);
                }

            }

            if (!isExistReady)
            {
                throw new InvalidOperationException(ExceptionMessages.BunniesNotReady);
            }

            List<IBunny> result = this.bunnyRepository.Models.Where(b => b.Energy == 0).ToList();
            if (result != null)
            {
                foreach (var item in result)
                {
                    this.bunnyRepository.Remove(item);
                }
            }

            if (egg.IsDone())
            {
                this.coloredEggs++;

                return string.Format(OutputMessages.EggIsDone, egg.Name);
            }
            else
            {

                return string.Format(OutputMessages.EggIsNotDone, egg.Name);
            }
        }

        public string Report()
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{this.coloredEggs} eggs are done!");
            if (this.bunnyRepository.Models.Count > 0)
            {
                sb.AppendLine($"Bunnys info:");
                foreach (var bunny in this.bunnyRepository.Models)
                {
                    
                        sb.AppendLine($"Name: {bunny.Name}");
                        sb.AppendLine($"Energy: {bunny.Energy}");
                        int result = bunny.Dyes.Where(d => d.Power > 0).Count();
                        sb.AppendLine($"Dyes: {result} not finished");
                    

                }
            }
            return sb.ToString().TrimEnd();

        }
    }
}
