// src/App.tsx
import React, { useState } from 'react';
import './App.css';
import { HomeScreen } from './components/HomeScreen';
import RegistrationForm from './components/RegistrationForm';

function App() {
  const [showRegistrationForm, setShowRegistrationForm] = useState(false);

  const handleToggleForm = () => {
    setShowRegistrationForm((prevState) => !prevState);
  };

  return (
    <div>
      <HomeScreen onToggleForm={handleToggleForm} showRegistrationForm={showRegistrationForm} />
    </div>
  );
}

export default App;
