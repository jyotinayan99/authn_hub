// App.jsx
import { Routes, Route } from "react-router-dom";
import LoginForm from "./components/LoginForm";
import Callback from "./components/Callback";
import Dashboard from "./components/Dashboard";

function App() {
  return (
    <Routes>
      <Route path="/" element={<LoginForm />} />
      <Route path="/callback" element={<Callback />} />
      <Route path="/dashboard" element={<Dashboard />} />
    </Routes>
  );
}

export default App;
