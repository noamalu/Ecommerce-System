import React, { useState, useEffect } from "react";
import { Container, Modal, Nav, Dropdown, Button, Col } from "react-bootstrap";
import { useNavigate } from 'react-router-dom';
import { CreateStore } from './CreateStore';
import ProfileStoreStuff from "./ProfileStoreStuff";
import ProfileStoreInfo from "./ProfileStoreInfo";

export const ProfileStoreNav = () => {
    const [num, setNum] = useState(1);
    const [showCreateStoreModal, setShowCreateStoreModal] = useState(false);
    const [view, setView] = useState<'ProfileStoreStuff' | 'ProfileStoreInfo'| 'Update Inventory' | 'Policies'>('ProfileStoreInfo');
    const [storeInfo, setStoreInfo] = useState<any>(null); // State to store the fetched store information

    const handleClose = () => setShowCreateStoreModal(false);
    const handleSuccess = () => {
        // handle success logic
    };
    const handleViewChange = (newView: 'ProfileStoreStuff' | 'ProfileStoreInfo'| 'Update Inventory' | 'Policies') => {
        setView(newView);
    };

    useEffect(() => {
        // Fetch store information when the component mounts or when the store ID changes
        fetchStoreInfo();
    }, [num]);

    const fetchStoreInfo = async () => {
        try {
            const response = await fetch(`https://localhost:7163/api/Market/Store/Name?storeId=${num}`);
            if (response.ok) {
                const data = await response.json();
                setStoreInfo(data);
            } else {
                throw new Error('Failed to fetch store information');
            }
        } catch (error) {
            console.error('Error fetching store information:', error);
        }
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
                <div>
                    <Button variant="primary" onClick={() => setShowCreateStoreModal(true)}>Create Store</Button>
                    <Dropdown className="ml-2">
                        <Dropdown.Toggle variant="secondary" id="dropdown-basic">
                            Options
                        </Dropdown.Toggle>
                        <Dropdown.Menu>
                            <Dropdown.Item onClick={() => handleViewChange('ProfileStoreStuff')}>Store Permission</Dropdown.Item>
                            <Dropdown.Item onClick={() => handleViewChange('Update Inventory')}>Update Inventory</Dropdown.Item>
                            <Dropdown.Item onClick={() => handleViewChange('Policies')}>Policies</Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                </div>
            </Container>
            <Container className="d-flex justify-content-between align-items-center my-3">
                <p className="mb-0">Store {num}</p>
            </Container>
            <Col className="profile-right">
                {view === 'ProfileStoreInfo' && <ProfileStoreInfo storeDetails={storeInfo} />} 
                {view === 'ProfileStoreStuff' && <ProfileStoreStuff />}
            </Col>
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
