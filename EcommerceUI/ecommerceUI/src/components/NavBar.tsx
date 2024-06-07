import React, {Component, useState} from "react";
import Button from 'react-bootstrap/Button';
import Container from 'react-bootstrap/Container';
import Form from 'react-bootstrap/Form';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';
import { useNavigate } from 'react-router-dom';
import SplitButton from 'react-bootstrap/SplitButton';
import Dropdown from 'react-bootstrap/Dropdown';



export const NavBar: React.FC = () => {
    const [query, setQuery] = useState('');
    const [selectedFilter, setSelectedFilter] = useState('name');
    const navigate = useNavigate();


    // Function to handle the event when an option is clicked
    const handleFilterClick = (option: string) => {
      // Update the selected option
      setSelectedFilter(option);
    };

    const onSearchClick = (event: React.FormEvent<HTMLFormElement>) => {
      console.log("onSearchClick");
      event.preventDefault();
      if (query.trim()) {
        navigate(`/search?query=${encodeURIComponent(query)}&filter=${encodeURIComponent(selectedFilter)}`);
      }
    };

    return (
        <Navbar expand="lg" className="bg-body-tertiary" fixed="top">
          <Container fluid>
            <Navbar.Brand href="/">Ecommerce</Navbar.Brand>
            <Navbar.Toggle aria-controls="navbarScroll" />
            <Navbar.Collapse id="navbarScroll">
              <Nav
                className="me-auto my-2 my-lg-0"
                style={{ maxHeight: '100px' }}
                navbarScroll
              >
                <Nav.Link href="/home">Home</Nav.Link>
                <NavDropdown title="Link" id="navbarScrollingDropdown">
                  <NavDropdown.Item href="#action3">Action</NavDropdown.Item>
                  <NavDropdown.Item href="#action4">
                    Another action
                  </NavDropdown.Item>
                  <NavDropdown.Divider />
                  <NavDropdown.Item href="#action5">
                    Something else here
                  </NavDropdown.Item>
                </NavDropdown>
              </Nav>
              <Form className="d-flex mx-auto" onSubmit={onSearchClick}>
                <Form.Control
                  type="search"
                  placeholder="Search"
                  className="me-2"
                  aria-label="Search"
                  value={query}
                  onChange={(ev) => setQuery(ev.target.value)}
                />
                    <SplitButton
                      key="search"
                      variant="outline-success"
                      type="submit"
                      title="search"
                    >
                      <Dropdown.Item eventKey="1" active={selectedFilter == "name"} onClick={() => handleFilterClick("name")}> name</Dropdown.Item>
                      <Dropdown.Item eventKey="2" active={selectedFilter == "category"} onClick={() => handleFilterClick("category")}> category </Dropdown.Item>
                      <Dropdown.Item eventKey="3" active={selectedFilter == "keyword"} onClick={() => handleFilterClick("keyword")}> keyword </Dropdown.Item>
                    </SplitButton>
                {/* <Button variant="outline-success" type="submit">Search</Button> */}
              </Form>
              <Button variant="outline-info" onClick={() => navigate('/login')}>Login</Button>
              <Button variant="outline-info" onClick={() => navigate('/register')}>Register</Button>
              <Button variant="outline-info" onClick={() => navigate('/profile')}>Profile</Button>
            </Navbar.Collapse>
          </Container>
        </Navbar>
      );
    
}





export default NavBar;