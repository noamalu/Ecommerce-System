import React, { useState } from 'react';
import {BrowserRouter, Routes, Route} from 'react-router-dom'
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import { Home } from "./pages/Home"
import { Login } from "./pages/Login"
import { NavBar } from "./components/NavBar"
import { Register } from './pages/Register';
import { Profile } from './pages/Profile';


function App() {
  const [loggedIn, setLoggedIn] = useState(false)
  const [username, setUsername] = useState('')

  return (
    <div>
      <NavBar />
      <BrowserRouter>
        <Routes>
          <Route index element = {<Home/>}/>
          <Route path="/home" element={<Home/>}/>
          <Route path="/login" element={<Login setLoggedIn={setLoggedIn} setUserName={setUsername} userr={username} />}  />
          <Route path="/Register" element={<Register/>}/>
          <Route path="/profile" element={<Profile setLoggedIn={setLoggedIn}/>}/>
        </Routes>
      </BrowserRouter>
       
    </div>
  );
}

export default App;
