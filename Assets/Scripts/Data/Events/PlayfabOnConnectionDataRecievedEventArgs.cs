namespace Assets.Scripts.Data.Events
{
    public class PlayfabOnConnectionDataRecievedEventArgs
    {
        public string NetworkId { get; set; }

        public PlayfabOnConnectionDataRecievedEventArgs(string networkId)
        {
            this.NetworkId = networkId;
        }

        public delegate void PlayfabOnConnectionDataRecievedEventHandler(object sender, PlayfabOnConnectionDataRecievedEventArgs eventArgs);
    }
}
