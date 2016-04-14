using System.Threading.Tasks;
using System.Windows.Input;
using FreshMvvm;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Taxi.Domain.Taximeter;
using Xamarin.Forms;
using Xamarin.Forms.Maps;


namespace Taxi.PageModels
{
    /// <summary>
    /// Map PageModel.
    /// </summary>
    public class MapPageModel : FreshBasePageModel
    {
        /// <summary>
        /// Gets or sets the start run command.
        /// </summary>
        /// <value>The start run command.</value>
        public ICommand StartRunCommand
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the center point.
        /// </summary>
        /// <value>The center point.</value>
        public Xamarin.Forms.Maps.Position CenterPoint
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the button text.
        /// </summary>
        /// <value>The button text.</value>
        public string ButtonText
        {
            get;
            set;
        } = "Start";

        /// <summary>
        /// Gets the cost display.
        /// </summary>
        /// <value>The cost display.</value>
        public string CostDisplay
        {
            get
            {
                return RunCost.ToString("C");
            } 
        }

        public decimal RunCost
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        /// <value>The radius.</value>
        public Distance Radius
        {
            get;
            set;
        } = Distance.FromKilometers(5);


        TaximeterService m_taximeter;


        /// <summary>
        /// Init the Page Model
        /// </summary>
        /// <param name="initData">Init data.</param>
        public override async void Init(object initData)
        {
            base.Init(initData);

            m_taximeter = new TaximeterService(FreshIOC.Container.Resolve<IGeolocator>());

            m_taximeter.TaxiMoved += (sender, e) => { RunCost = ((TaximeterService)sender).RunCost; };
            m_taximeter.RunStarted += (sender, e) => { ButtonText = "Stop"; };
            m_taximeter.RunStoped += (sender, e) => { ButtonText = "Start"; };

            await AskPersmissionToUseGPS();

            RunCost = 0;
            StartRunCommand = new Command(StartRun);
            SetCenterPointToCurrentLocation();
        }

        /// <summary>
        /// Starts the run.
        /// </summary>
        /// <returns>The run.</returns>
        void StartRun()
        {
            SetCenterPointToCurrentLocation();
            StartRunCommand = new Command(StopRun);
            m_taximeter.StartRun();
        }

        /// <summary>
        /// Stops the run.
        /// </summary>
        /// <returns>The run.</returns>
        void StopRun()
        {
            m_taximeter.StopRun();
            StartRunCommand = new Command(StartRun);
            RunCost = m_taximeter.RunCost;
        }

        async void SetCenterPointToCurrentLocation()
        {
            var position = await m_taximeter.GetCurrentLocation();
            CenterPoint = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
        }

        /// <summary>
        /// Asks the persmission to use gps.
        /// </summary>
        /// <returns>The persmission to use gps.</returns>
        async Task AskPersmissionToUseGPS()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                {
                    await CoreMethods.DisplayAlert("Need location", "Gunna need that location", "Cancel");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                status = results[Permission.Location];
            }
        }
    }
}

