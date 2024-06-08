import React, {Component, useState} from "react";
import { useNavigate } from "react-router-dom";
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';


interface Login{
  setLoggedIn: (arg0: boolean) => void;
  setUsername: (arg0: string) => void;
  userr: string;
}

export const Login = (props: { setLoggedIn: (arg0: boolean) => void; setUserName: (arg0: string) => void; userr: string }) => {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [usernameError, setUsernameError] = useState('')
  const [passwordError, setPasswordError] = useState('')

  const navigate = useNavigate()

  const onButtonClick = () => {
    // Set initial error values to empty
  setUsernameError('')
  setPasswordError('')
  
  // Check if the user has entered both fields correctly
  if ('' === username ) {
    setUsernameError('Please enter your username')
    return
  }


  if ('' === password) {
    setPasswordError('Please enter a password')
    return
  }

  if (password.length < 7) {
    setPasswordError('The password must be 8 characters or longer')
    return
  }
  
  logIn()
   
  }
  
  const logIn = () => {
    var tokenId = 123;
    fetch(`https://localhost:7163/api/Client/Guest/Login?tokenId=${tokenId}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({username, password }),
    })
      .then((r) => r.json())
      .then((r) => {
        if ('success' === r.message) {
          localStorage.setItem('user', JSON.stringify({ username, token: r.token }))
          props.setLoggedIn(true)
          props.setUserName(username)
          navigate('/')
        } else {
          window.alert('Wrong email or password')
        }
      })
  }


  return (
    <>
    <Form.Group className="mb-3">
      <Form.Label>Username</Form.Label>
      <Form.Control type="text"
        name="username"
        value={username}
        onChange={(ev) => setUsername(ev.target.value)} 
        placeholder="username" />
    </Form.Group>
    <label className="errorLabel">{usernameError}</label>

    <Form.Group className="mb-3" >
      <Form.Label>Password</Form.Label>
      <Form.Control type="password" placeholder="Password" 
                    name="password"
                    value={password}
                    onChange={(ev) => setPassword(ev.target.value)}
                    />
    </Form.Group>
    <label className="errorLabel">{passwordError}</label>
    <Button variant="primary" type="submit" onClick={onButtonClick}>
      Login 
    </Button>
     </> 
    );
}


