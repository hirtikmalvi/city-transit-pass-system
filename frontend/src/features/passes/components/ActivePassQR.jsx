import { mockActivePass } from '../../../utils/mockData';

const ActivePassQR = () => {
  // If the user has no active pass, we don't render this ticket
  if (!mockActivePass || mockActivePass.status !== 'Active') return null;

  // Format the expiry date to look nice (e.g., "15 Apr 2026, 10:30 AM")
  const formattedExpiry = new Date(mockActivePass.expiry_date).toLocaleDateString('en-IN', {
    day: 'numeric', month: 'short', year: 'numeric',
    hour: '2-digit', minute: '2-digit'
  });

  return (
    <div className="mb-10 bg-white rounded-2xl shadow-lg border-t-8 border-green-500 p-6 flex flex-col md:flex-row items-center justify-between">
      
      {/* Ticket Details */}
      <div className="flex-1 mb-6 md:mb-0">
        <span className="bg-green-100 text-green-800 text-xs font-bold px-3 py-1 rounded-full uppercase tracking-wide">
          Active Pass
        </span>
        <h2 className="text-3xl font-extrabold text-gray-900 mt-3">
          {mockActivePass.pass_name}
        </h2>
        <p className="text-gray-500 mt-2">
          Valid until: <span className="font-bold text-red-500">{formattedExpiry}</span>
        </p>
      </div>

      {/* The "QR" Code Area */}
      <div className="bg-gray-50 p-4 rounded-xl border-2 border-dashed border-gray-300 flex flex-col items-center justify-center w-full md:w-64 h-48">
        <div className="w-20 h-20 bg-gray-200 mb-3 flex items-center justify-center rounded-lg shadow-inner">
          <span className="text-4xl">📱</span>
        </div>
        <p className="font-mono text-sm font-bold text-gray-800 tracking-widest">
          {mockActivePass.pass_code}
        </p>
        <p className="text-xs text-gray-400 mt-1 uppercase font-semibold">Show to Conductor</p>
      </div>

    </div>
  );
};

export default ActivePassQR;