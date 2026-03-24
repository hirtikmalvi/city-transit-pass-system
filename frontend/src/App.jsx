import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import ProtectedRoute from './features/auth/components/ProtectedRoute';
import { useAuth } from './features/auth/AuthContext';
import CommuterDashboard from './pages/CommuterDashboard';
import Login from './pages/Login';
import Register from './pages/Register';

function App() {
  const { isAuthenticated } = useAuth();

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Navigate to={isAuthenticated ? '/commuter' : '/login'} replace />} />
        <Route path="/login" element={isAuthenticated ? <Navigate to="/commuter" replace /> : <Login />} />
        <Route path="/register" element={isAuthenticated ? <Navigate to="/commuter" replace /> : <Register />} />
        <Route
          path="/commuter"
          element={
            <ProtectedRoute>
              <CommuterDashboard />
            </ProtectedRoute>
          }
        />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
