import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import CommuterDashboard from './pages/CommuterDashboard';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Automatically redirect the home page to your commuter dashboard for now */}
        <Route path="/" element={<Navigate to="/commuter" replace />} />
        
        {/* Your Commuter Route */}
        <Route path="/commuter" element={<CommuterDashboard />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;