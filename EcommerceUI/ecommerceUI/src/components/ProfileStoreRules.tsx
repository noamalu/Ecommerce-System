import React, { useEffect, useState } from 'react';
import { Table, Dropdown, Form, Stack, Button, Card, Row, Col, Container, Modal } from 'react-bootstrap';
import { getToken } from '../services/SessionService';
import { RiSave2Fill, RiAddBoxFill} from 'react-icons/ri'; // Importing the shopping cart icon
import { CreateRule } from './CreateRule';


export const ProfileStoreRules = ({storeId} : {storeId : any}) => {
    const [rules, setRules] = useState<any[]>([]);
    const [showAddRuleModal, setShowAddRuleModal] = useState(false);

    const handleClose = () => setShowAddRuleModal(false);
    const handleSuccess = () => {
        window.location.reload();

    };

    useEffect(() => {
        const fetchPurchaseHistory = async () => {

            try {
                //TODO change the url token id
                const response = await fetch(`https://localhost:7163/api/Market/Store/${storeId}/GetRules?identifier=${getToken()}`, {
                    method: 'GET'
                });
                const data = await response.json();
                if (response.ok) {
                    // console.log(data.value);
                    // const newPurchases = data.value[0].baskets.flatMap((basket: any) => basket.products);
            // console.log(newPurchases);
                    setRules(data.value);
                } else {
                    console.error('Error fetching purchase history:', data.ErrorMessage);
                }
            } catch (error) {
                console.error('Error occurred while fetching purchase history:', error);
            }
        };

        fetchPurchaseHistory();
    }, []); 



    return (
        <>
        <Container className="my-3">
            <h2>Store Rules</h2>
            <Table striped bordered hover className="my-3">
                <thead>
                    <tr>
                        <th>Rule ID</th>
                        <th>Rule Subject</th>
                    </tr>
                </thead>
                <tbody>
                    {rules.map((rule) => (
                        <tr>
                            <td>{rule.id}</td>
                            <td>{rule.subjectInfo}</td>
                        </tr>
                    ))}
                </tbody>
            </Table>
            <Button variant="outline-success" onClick={() => setShowAddRuleModal(true)}> <RiAddBoxFill size={20} /></Button>

            <Modal show={showAddRuleModal} onHide={handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>Add Rule</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <CreateRule onClose={handleClose} onSuccess={handleSuccess} storeId={storeId}/>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Close
                    </Button>
                </Modal.Footer>
            </Modal>

        </Container>
        </>
    );
};

export default ProfileStoreRules;
