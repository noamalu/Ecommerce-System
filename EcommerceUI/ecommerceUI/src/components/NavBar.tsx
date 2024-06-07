import Button from 'react-bootstrap/Button';
import Container from 'react-bootstrap/Container';
import Form from 'react-bootstrap/Form';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';
import { RiShoppingCart2Line, RiUserLine } from 'react-icons/ri'; // Importing the shopping cart icon


interface NavBar {
}

export const NavBar: React.FC<NavBar> = () => {
    return (
        <Navbar expand="lg" className="bg-body-tertiary" fixed="top">
          <Container fluid>
          <RiShoppingCart2Line size={30} />
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
              <Form className="d-flex mx-auto">
                <Form.Control
                  type="search"
                  placeholder="Search"
                  className="me-2"
                  aria-label="Search"
                />
                <Button variant="outline-success">Search</Button>
              </Form>
              <Button variant="outline-info" href="/login">Login</Button>
              <Button variant="outline-info" href="/register">Register</Button>
              <Button variant="outline-info" href="/profile"><RiUserLine size={20} /></Button>
            </Navbar.Collapse>
          </Container>
        </Navbar>
      );
    
}

export default NavBar;