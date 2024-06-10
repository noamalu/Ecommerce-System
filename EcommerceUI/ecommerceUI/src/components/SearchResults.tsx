import React, {Component, useState, useEffect} from "react";
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import  Container  from "react-bootstrap/Container";
import ItemCard from "./ItemCard";


interface SearchResults {
    query: string;
    filter: string;
}

export const SearchResults = ({query, filter}) => {
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
                console.log(dataValue);
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
            <Container className="flex small-padding">
                { <Row xs={1} md={Math.min(dataValue.length, 4)} className="g-4">
                {dataValue.map((product) => (
                    <Col key={product.id}>
                    <ItemCard itemName={product.name}
                        description={product.description}
                        price={product.price} addToCart={function (storeId: number, productId: number): void {
                            throw new Error("Function not implemented.");
                        // } } storeId={product.storeId} productId={product.id}  />
                        } } storeId={1} productId={product.id}  />
                    </Col>
                ))}
              </Row> }
            </Container>
        </>
    );
    
}


export default SearchResults;
