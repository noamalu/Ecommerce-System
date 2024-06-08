import React, {Component, useState} from "react";
import { Container, Modal } from "react-bootstrap";
import Image from 'react-bootstrap/Image';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import Button from 'react-bootstrap/Button';
import Stack from 'react-bootstrap/Stack';
import Nav from 'react-bootstrap/Nav';
import avatar from '../assets/avatar.jpg';
import Table from 'react-bootstrap/Table';
import Dropdown from 'react-bootstrap/Dropdown';
import Form from 'react-bootstrap/Form';
import InputGroup from 'react-bootstrap/InputGroup';
import { useNavigate } from 'react-router-dom';
import { CreateStore } from './CreateStore';



export const ProfileStoreNav = () => {
    const [num, setNum] = useState(1);
    const [showCreateStoreModal, setShowCreateStoreModal] = useState(false);

    const handleClose = () => setShowCreateStoreModal(false);
    const handleSuccess = () => {
    };
    
    return (
        <>
            <Container className="d-flex justify-content-between align-items-center my-3">
                <Nav variant="tabs" defaultActiveKey="/home">
                    <Nav.Item>
                        <Nav.Link href="" onClick={() => setNum(1)}>Store 1</Nav.Link>
                    </Nav.Item>
                    <Nav.Item>
                        <Nav.Link onClick={() => setNum(2)}>Store 2</Nav.Link>
                    </Nav.Item>
                </Nav>
                <Button variant="primary" onClick={() => setShowCreateStoreModal(true)}>Create Store</Button>
            </Container>
            <Container className="d-flex justify-content-between align-items-center my-3">
                <p className="mb-0">Store {num}</p>
            </Container>
            <Table striped bordered hover className="my-3">
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Name</th>
                        <th>Phone Number</th>
                        <th>Role</th>
                        <th>Appointer</th>
                        <th>Permissions</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>1</td>
                        <td>Mark</td>
                        <td>+972 52-123-4567</td>
                        <td>Founder</td>
                        <td></td>
                        <td>
                            <Dropdown>
                                <Dropdown.Toggle variant="success" id="dropdown-basic">
                                    Dropdown Button
                                </Dropdown.Toggle>
                                <Dropdown.Menu>
                                    <Dropdown.Item href="#/action-1">
                                        <Form>
                                            <div key="default-checkbox" className="mb-3">
                                                <Form.Check
                                                    type="checkbox"
                                                    id="default-checkbox"
                                                    label="Permission 1"
                                                />
                                                <Form.Check
                                                    type="checkbox"
                                                    id="default-checkbox"
                                                    label="Permission 2"
                                                />
                                                <Form.Check
                                                    type="checkbox"
                                                    id="default-checkbox"
                                                    label="Permission 3"
                                                />
                                            </div>
                                        </Form>
                                    </Dropdown.Item>
                                </Dropdown.Menu>
                            </Dropdown>
                        </td>
                    </tr>
                </tbody>
            </Table>

            <Modal show={showCreateStoreModal} onHide={handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>Create Store</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <CreateStore onClose={handleClose} onSuccess={handleSuccess} />
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Close
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
};
    
    export default ProfileStoreNav;