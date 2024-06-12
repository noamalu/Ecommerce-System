import React, {Component, useState, useEffect} from "react";
import Table from 'react-bootstrap/Table';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import Container from 'react-bootstrap/Container';
import { getToken } from "../services/SessionService";
import { Stack } from 'react-bootstrap';


const handleChange = (e: any, product: {storeId: number, id: number, productName: string, quantity: number}) => {
    const newQuantity = e.target.value;
    console.log(product.quantity);
    console.log(newQuantity);
    var multiplier = 1;
    if (newQuantity < product.quantity)
        multiplier = -1;


    fetch(`https://localhost:7163/api/Client/Cart?identifier=${getToken()}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
                    storeId: product.storeId,
                    id: product.id,
                    quantity: multiplier,
                  })
      }).then((r) => {
        if (r.ok) {
          console.log("incrementing quantity worked");
          window.location.reload(); //refresh the page to show updated items and quantities
        } else {
          throw new Error('incrementing quantity didnt work');
        }
      })
    //   .catch((error) => {
    //     window.alert(error.message);
    //   });

};

const getStoreName = async (storeId: number): Promise<string> => {
    try {
        const response = await fetch(`https://localhost:7163/api/Market/Store/Name?identifier=${getToken()}&storeId=${storeId}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (!response.ok) {
            throw new Error('Error occurred in getting store name');
        }

        const data = await response.json();
        return data.value;
    } catch (error) {
        console.error('Error:', error);
        return "undefined";
    }
};



export const Cart = () => {
    const [dataValue, setDataValue] = useState<any[]>([]);
    const [price, setPrice] = useState<number>(0);

    //get cart information
    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch(`https://localhost:7163/api/Client/Cart?identifier=${getToken()}`, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                });

                if (!response.ok) {
                    throw new Error('Error occurred in getting cart information');
                }

                const data = await response.json();
                console.log(data);
                setPrice(data.value.price);
                const newProducts = data.value.baskets.flatMap((basket: any) => basket.products);
                const updatedDataValue = await Promise.all(newProducts.map(async (item: any) => {
                    const storeName = await getStoreName(item.storeId);
                    return { ...item, storeName }; // Add storeName to each item
                }));

                setDataValue(updatedDataValue);
            } catch (error) {
                console.error('Error:', error);
            }
        };

        fetchData();
    }, []);


    return (
        <div className="small-padding">
            <Stack direction="horizontal" gap={3} className="half-width">
                <div className="p-2"> Total: {price}₪</div>
                <div className="p-2 ms-auto"></div>
                <div className="p-2">
                    <Button className="align-right" onClick={() => alert("clicked on purchase cart")}> Purchase cart </Button>
                </div>
            </Stack>
            <Table striped bordered hover className="my-3 full-width">
                <thead>
                    <tr>
                        <th>Product Name</th>
                        <th>Store Name</th>
                        <th>Price</th>
                        <th>Quantity</th>
                    </tr>
                </thead>
                <tbody>                
                    {dataValue.map((product) => (
                    <tr>
                    <td>{product.name}</td>
                    <td>{product.storeName}</td>
                    <td>{product.price}₪</td>
                    <td> <Form.Control
                        type="number"
                        className="mx-2"
                        value={product.quantity}
                        onChange={(e) => handleChange(e, {storeId: product.storeId, id: product.id, productName: product.name, quantity: product.quantity})}
                        min="0"
                        style={{ width: '60px', textAlign: 'center' }}
                    /> </td>
                </tr>
                ))}
                </tbody>
            </Table>
        </div>
    );
    
}