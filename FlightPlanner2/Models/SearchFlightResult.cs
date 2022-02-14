using System.Linq;

namespace FlightPlanner2.Models
{
    public class SearchFlightResult
    {
        private readonly Flight[] _items;
        private readonly int _page;
        private readonly int _totalItems;

        public SearchFlightResult(Flight[] items)
        {
            _items = items;
            _page = _items.Length > 0 ? 1 : 0;
            _totalItems = _items.Length;
        }

        public Flight[] items => _items.ToArray();
        public int page => _page;
        public int totalItems => _totalItems;
    }
}
