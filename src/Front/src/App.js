import { StrictMode } from "react";
import { Routes, Route } from 'react-router-dom'
import Home from './views/Home';
import NotFound from './views/NotFound';
import Login from "./views/Login";
import { ProtectedRoute } from "./components/ProtectedRoute ";

function App() {
  return (
    <StrictMode>      
        <Routes>
          <Route element={<ProtectedRoute />}>
            <Route path="/" element={<Home />} />
          </Route>
          <Route path='/login' element={<Login />} />
          <Route path='*' element={<NotFound />} />
        </Routes>
      </StrictMode>
  );
}

export default App;
