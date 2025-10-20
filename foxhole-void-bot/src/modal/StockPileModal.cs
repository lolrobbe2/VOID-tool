using FoxholeBot.repositories;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace FoxholeBot.modal
{
    public class StockpileModule : ComponentInteractionModule<StringMenuInteractionContext>
    {
        public static string selected {  get; set; }
        [ComponentInteraction("select_region")]
        public string Region() {
            selected = Context.SelectedValues.First();
            return selected;
        }
    }
}
