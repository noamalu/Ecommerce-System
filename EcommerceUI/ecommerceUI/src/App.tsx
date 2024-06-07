import React, { useState } from 'react';
import {BrowserRouter, Routes, Route} from 'react-router-dom'
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import { Home } from "./pages/Home"
import { Login } from "./pages/Login"
import { NavBar } from "./components/NavBar"
import { Register } from './pages/Register';
import { Profile } from './pages/Profile';
import { initSession } from './services/SessionService';
import { Search } from './pages/Search';
import { Cart } from './pages/Cart';


function App() {
  initSession();
  const [loggedIn, setLoggedIn] = useState(false)
  const [username, setUsername] = useState('')

  return (
    <div>
      
      <BrowserRouter>
        <NavBar />
        <Routes>
          <Route index element = {<Home/>}/>
          <Route path="/home" element={<Home/>}/>
          <Route path="/login" element={<Login setLoggedIn={setLoggedIn} setUserName={setUsername} userr={username} />}  />
          <Route path="/Register" element={<Register/>}/>
          <Route path="/search" element={<Search/>}/>
          <Route path="/profile" element={<Profile setLoggedIn={setLoggedIn}/>}/>
          <Route path="/cart" element={<Cart/>}/>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
