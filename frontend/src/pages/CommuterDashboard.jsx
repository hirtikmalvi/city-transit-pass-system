import { useNavigate } from 'react-router-dom';
import { useAuth } from '../features/auth/AuthContext';
import ActivePassQR from '../features/passes/components/ActivePassQR';
import PassCatalogList from '../features/passes/components/PassCatalogList';

const CommuterDashboard = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login', { replace: true });
  };

  return (
    <div className="min-h-screen bg-gray-50 p-8">
      <header className="mb-8 flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
        <div>
          <h1 className="text-3xl font-extrabold text-gray-900">
            Welcome, {user?.name || 'Commuter'}! {'\uD83D\uDE86'}
          </h1>
          <p className="text-gray-600">
            Signed in as {user?.email || 'your commuter account'}.
          </p>
        </div>

        <button
          type="button"
          onClick={handleLogout}
          className="rounded-xl bg-slate-900 px-4 py-3 text-sm font-semibold text-white transition hover:bg-slate-800"
        >
          Logout
        </button>
      </header>

      <main className="mx-auto max-w-6xl">
        <ActivePassQR />
        <PassCatalogList />
      </main>
    </div>
  );
};

export default CommuterDashboard;
