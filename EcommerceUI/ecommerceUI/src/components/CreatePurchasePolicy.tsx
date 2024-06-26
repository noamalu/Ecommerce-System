import React, { useState, useEffect } from 'react';
import { Form, Button, Row, Col } from 'react-bootstrap';
import { getToken } from '../services/SessionService';
import 'react-datepicker/dist/react-datepicker.css';
import DatePicker from 'react-datepicker';


export const CreatePurchasePolicy = ({ onClose, onSuccess, storeId }: { onClose: any, onSuccess: any, storeId: any }) => {
    const [rules, setRules] = useState<any[]>([]);
    
    const [ruleString, setRuleString] = useState('');
    const [rule, setRule] = useState({
        "id": 0,
        "subjectInfo": ""
      });
      const [selectedDate, setSelectedDate] = useState(null);

    useEffect(() => {
        const fetchRulesList = async () => {

            try {
                const response = await fetch(`https://localhost:7163/api/Market/Store/${storeId}/GetRules?identifier=${getToken()}`, {
                    method: 'GET'
                });
                const data = await response.json();
                if (response.ok) {
                    setRules(data.value);
                } else {
                    console.error('Error fetching rules list:', data.ErrorMessage);
                }
            } catch (error) {
                console.error('Error occurred while fetching rules list:', error);
            }
        };

        fetchRulesList();
    }, []); 

    const addPolicy = async () => {
        const response = await fetch(`https://localhost:7163/api/Market/Store/${storeId}/Policies/Purchace?identifier=${getToken()}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({expirationDate: selectedDate, subject: rule.subjectInfo, ruleId: rule.id})
        })
        if (response.ok) {
            alert('Policy added successfully');
        } else {
            const responseData = await response.json();
            console.error('Failed to add Policy:', responseData);
            alert('Failed to add Policy. Please try again later.');
        }
        window.location.reload();
    }        

                
        return (
            <Form>
                <Form.Group as={Col} controlId="">
                        <Form.Label>Rule</Form.Label>
                        <Form.Select name="ruleType" value={ruleString} onChange={(e) => {
                                const [id, subjectInfo] = e.target.value.split(',');
                                setRule({ id: parseInt(id), subjectInfo });
                                setRuleString(e.target.value);
                            }}>
                        {rules.map((rule) => (
                            <option value={`${rule.id},${rule.subjectInfo}`}>{rule.id}: {rule.subjectInfo}</option>
                    ))}
                        </Form.Select>
                    </Form.Group>

                    <Form.Group>
                        <Form.Label>Select Date</Form.Label>
                        <DatePicker
                            selected={selectedDate}
                            onChange={(date) => setSelectedDate(date)}
                            className="form-control"
                        />
                    </Form.Group>
                
                <Button variant="primary" type="button" onClick={addPolicy}>
                    Add Rule
                </Button>
            </Form>
        );

};
