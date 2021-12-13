namespace ShopHttp.ShopHttpServer.Services

{
    public class NotifyService
    {
        public delegate void EventHandler();

        public event EventHandler CreateShowcaseIsDone;
        public event EventHandler CreateProductIsDone;
        public event EventHandler PlaceProductIsDone;
        public event EventHandler EditProductIsDone;
        public event EventHandler DeleteProductIsDone;
        public event EventHandler EditShowcaseIsDone;
        public event EventHandler DeleteShowcaseIsDone;
        public event EventHandler VolumeError;
        public event EventHandler ProductIsNotfound;
        public event EventHandler SearchProductIdIsNotSuccessful;
        public event EventHandler CountCheck;
        public event EventHandler DeleteError;
        public event EventHandler ChekProductOnShowacse;
        public event EventHandler ArchivateProductIsDone;
        public event EventHandler UnArchivateProductIsDone;
        public event EventHandler DeleteArchiveProductIsDone;
        public event EventHandler ArchiveIsEmpty;

        public void RaiseCreateShowcaseIsDone()
        {
            CreateShowcaseIsDone?.Invoke();
        }

        public void RaiseCreateProductIsDone()
        {
            CreateProductIsDone?.Invoke();
        }

        public void RaisePlaceProductIsDone()
        {
            PlaceProductIsDone?.Invoke();
        }

        public void RaiseEditProductIsDone()
        {
            EditProductIsDone?.Invoke();
        }

        public void RaiseDeleteProductIsDone()
        {
            DeleteProductIsDone?.Invoke();
        }

        public void RaiseEditShowcaseIsDone()
        {
            EditShowcaseIsDone?.Invoke();
        }

        public void RaiseDeleteShowcaseIsDone()
        {
            DeleteShowcaseIsDone?.Invoke();
        }

        public void RaiseVolumeErrorMessage()
        {
            VolumeError?.Invoke();
        }

        public void RaiseProductIsNotfound()
        {
            ProductIsNotfound?.Invoke();
        }

        public void RaiseSearchProductIdIsNotSuccessful()
        {
            SearchProductIdIsNotSuccessful?.Invoke();
        }

        public void RaiseCountCheck()
        {
            CountCheck?.Invoke();
        }

        public void RaiseDeleteError()
        {
            DeleteError?.Invoke();
        }

        public void RaiseChekProductOnShowacse()
        {
            ChekProductOnShowacse?.Invoke();
        }

        public void RaiseArchiveIsEmpty()
        {
            ArchiveIsEmpty?.Invoke();
        }

        public void RaiseArchivateProductIsDone()
        {
            ArchivateProductIsDone?.Invoke();
        }

        public void RaiseUnArchivateProductIsDone()
        {
            UnArchivateProductIsDone?.Invoke();
        }

        public void RaiseDeleteArchiveProductIsDone()
        {
            DeleteArchiveProductIsDone?.Invoke();
        }
    }
}
