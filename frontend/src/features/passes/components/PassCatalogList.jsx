import { mockPassTypes } from '../../../utils/mockData';

const PassCatalogList = () => {
  return (
    <div className="p-6">
      <h2 className="text-2xl font-bold mb-6 text-gray-800">Available Transit Passes</h2>
      
      {/* Grid container for the cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        
        {/* Loop through our dummy database data */}
        {mockPassTypes.map((pass) => (
          <div 
            key={pass.id} 
            className="bg-white rounded-xl shadow-md overflow-hidden border border-gray-100 hover:shadow-lg transition-shadow"
          >
            {/* Card Header */}
            <div className="bg-blue-600 p-4">
              <h3 className="text-xl font-bold text-white">{pass.name}</h3>
            </div>
            
            {/* Card Body */}
            <div className="p-5">
              <p className="text-4xl font-extrabold text-gray-900 mb-4">
                ₹{pass.price}
              </p>
              
              <ul className="text-sm text-gray-600 space-y-2 mb-6">
                <li className="flex justify-between">
                  <span className="font-semibold text-gray-500">Validity:</span> 
                  <span>{pass.validity_days} {pass.validity_days === 1 ? 'Day' : 'Days'}</span>
                </li>
                <li className="flex justify-between">
                  <span className="font-semibold text-gray-500">Daily Limit:</span> 
                  <span>{pass.max_trips_per_day ? `${pass.max_trips_per_day} Trips` : 'Unlimited'}</span>
                </li>
                <li className="flex justify-between">
                  <span className="font-semibold text-gray-500">Access:</span> 
                  <span className="text-right">{pass.transport_modes.join(' + ')}</span>
                </li>
              </ul>
              
              {/* Fake Purchase Button for now */}
              <button 
                onClick={() => alert(`Purchase flow for ${pass.name} coming soon!`)}
                className="w-full bg-blue-50 text-blue-700 font-bold py-3 px-4 rounded-lg hover:bg-blue-100 transition-colors"
              >
                Buy Now
              </button>
            </div>
          </div>
        ))}
        
      </div>
    </div>
  );
};

export default PassCatalogList;