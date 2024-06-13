import React, { useState } from 'react';
import { Form, Button, Row, Col } from 'react-bootstrap';
import { getToken } from '../services/SessionService';

export const CreateRule = ({ onClose, onSuccess, storeId }: { onClose: any, onSuccess: any, storeId: any }) => {
    const [ruleType, setRuleType] = useState('SimpleRule');
    const [subject, setSubject] = useState('');
    const [minQuantity, setMinQuantity] = useState('');
    const [maxQuantity, setMaxQuantity] = useState('');
    const [targetPrice, setTargetPrice] = useState('');

    const [ruleTypeError, setRuleTypeError] = useState('');
    const [subjectError, setSubjectError] = useState('');
    const [minQuantityError, setMinQuantityError] = useState('');
    const [maxQuantityError, setMaxQuantityError] = useState('');
    const [targetPriceError, setTargetPriceError] = useState('');


    const addRule = async () => {
        var ruleTypeAddress = "";
        var body;
        switch (ruleType) {
            case 'Quantity': {ruleTypeAddress = `/Quantity`; 
                            body = JSON.stringify({subject: subject,
                                                    minQuantity: minQuantity,
                                                    maxQuantity: maxQuantity,
                                                }); 
                            break; }
            case 'TotalPrice': {ruleTypeAddress = `/TotalPrice`; 
                                body = JSON.stringify({subject: subject,
                                                        targetPrice: targetPrice,
                                                    }); 
                                break; }
            case 'SimpleRule': {ruleTypeAddress = `/TotalPrice`; 
                                body = JSON.stringify({subject: subject}); 
                                break; }
        }

        const response = await fetch(`https://localhost:7163/api/Market/Store/${storeId}/AddRule${ruleTypeAddress}?identifier=${getToken()}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: body
        })
        if (response.ok) {
            alert('Rule added successfully');
        } else {
            const responseData = await response.json();
            console.error('Failed to add rule:', responseData);
            alert('Failed to add rule. Please try again later.');
        }
        window.location.reload();
        };

        const validate = () => {
            // Validate the inputs
            let valid = true;
            
            if (subject.trim() === '') {
                setSubjectError('Please enter the subject');
                valid = false;
            }

            switch (ruleType) {
                case 'Quantity': { 
                                    if (minQuantity.trim() === '') {
                                        setMinQuantity('Please enter a minimum quantity');
                                        valid = false;
                                    }
                            
                                    if (maxQuantity.trim() === '') {
                                        setMaxQuantity('Please enter a maximum quantity');
                                        valid = false;
                                    }
                                break; }
                case 'TotalPrice': { 
                                    if (targetPrice.trim() === '') {
                                        setTargetPrice('Please enter a desired total price');
                                        valid = false;
                                    }
                                break; }
            }

            if(valid)
                addRule();
        }
    
        return (
            <Form>
                <Row>
                <Col>
                <Form.Group as={Col} controlId="">
                        <Form.Label>Rule Type</Form.Label>
                        <Form.Select name="ruleType" value={ruleType} onChange={(e) => setRuleType(e.target.value)}>
                        <option value="SimpleRule">Simple Rule</option>
                        <option value="Quantity">Quantity</option>
                        <option value="TotalPrice">Total Price</option>
                        </Form.Select>
                    </Form.Group>
                </Col>
                <Col>
                <Form.Group className="mb-3">
                    <Form.Label>Subject</Form.Label>
                    <Form.Control 
                        type="text"
                        value={subject}
                        onChange={(ev) => setSubject(ev.target.value)} 
                        placeholder="Enter Subject" 
                    />
                    <label className="errorLabel">{subjectError}</label>
                </Form.Group>
                </Col>
                </Row>

                <Row>
                    <Col>
                <Form.Group className="mb-3">
                    <Form.Label>Min Quantity</Form.Label>
                    <Form.Control 
                        type="text"
                        value={minQuantity}
                        onChange={(ev) => setMinQuantity(ev.target.value)} 
                        placeholder="" 
                        disabled={ruleType != 'Quantity'}
                    />
                    { ruleType == 'Quantity' && <label className="errorLabel">{minQuantityError}</label>}
                </Form.Group>
                </Col>
                <Col>
                <Form.Group className="mb-3">
                    <Form.Label>Max Quantity</Form.Label>
                    <Form.Control 
                        type="text"
                        value={maxQuantity}
                        onChange={(ev) => setMaxQuantity(ev.target.value)} 
                        placeholder="" 
                        disabled={ruleType != 'Quantity'}
                    />
                    { ruleType == 'Quantity' && <label className="errorLabel">{maxQuantityError}</label> }
                </Form.Group>
                </Col>
                <Col>
                <Form.Group className="mb-3">
                    <Form.Label>Total Price</Form.Label>
                    <Form.Control 
                        type="text"
                        value={targetPrice}
                        onChange={(ev) => setTargetPrice(ev.target.value)} 
                        placeholder="" 
                        disabled={ruleType != 'TotalPrice'}
                    />
                    {ruleType == 'TotalPrice' && <label className="errorLabel">{targetPriceError}</label> }
                </Form.Group>
                </Col>
                </Row>
                
                <Button variant="primary" type="button" onClick={validate}>
                    Add Rule
                </Button>
            </Form>
        );

};
