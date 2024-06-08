import React, {Component, useState, useEffect} from "react";
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import Button from 'react-bootstrap/Button';
import Stack from 'react-bootstrap/Stack';
import  Container  from "react-bootstrap/Container";
import { useLocation } from 'react-router-dom';
import ItemCard from "../components/ItemCard";
import parse from 'html-react-parser';




export const Search = () => {
    const location = useLocation();
    const query = new URLSearchParams(location.search).get('query');
    const filter = new URLSearchParams(location.search).get('filter');
    const [dataValue, setDataValue] = useState<any[]>([]); // Declare a state to store data.value
    var tokenId = 123;

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch(`https://localhost:7163/api/Market/Search/${filter}?tokenId=${tokenId}&${filter}=${query}`, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                });

                if (!response.ok) {
                    throw new Error('Error occurred in search');
                }

                const data = await response.json();
                setDataValue(data.value);
            } catch (error) {
                console.error('Error:', error);
            }
        };

        if (query && filter) {
            fetchData();
        }
    }, [query, filter]);

    return(
        <>
            <Container className="flex">
                { <Row xs={1} md={Math.min(dataValue.length, 4)} className="g-4">
                {dataValue.map((product) => (
                    <Col key={product.id}>
                    <ItemCard itemName={product.name}
                        description={product.description}
                        price={product.price} addToCart={function (storeId: number, productId: number): void {
                            throw new Error("Function not implemented.");
                        } } storeId={product.storeId} productId={product.id}  />
                    </Col>
                ))}
              </Row> }
            </Container>
        </>
    );

}