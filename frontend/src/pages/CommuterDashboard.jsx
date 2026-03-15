import ActivePassQR from '../features/passes/components/ActivePassQR';
import PassCatalogList from '../features/passes/components/PassCatalogList';

const CommuterDashboard = () => {
  return (
    <div className="min-h-screen bg-gray-50 p-8">
      <header className="mb-8">
        <h1 className="text-3xl font-extrabold text-gray-900">Welcome, Charlie! 🚆</h1>
        <p className="text-gray-600">Buy a new pass or manage your active journeys.</p>
      </header>
      
      <main className="max-w-6xl mx-auto">
        {/* The Digital Ticket goes at the very top! */}
        <ActivePassQR />
        
        {/* The Storefront goes underneath */}
        <PassCatalogList />
      </main>
    </div>
  );
};

export default CommuterDashboard;