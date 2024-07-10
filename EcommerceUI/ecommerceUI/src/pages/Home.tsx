import React, {Component} from "react";
import SearchResults from "../components/SearchResults";


export const Home = () => {
    return (
        <SearchResults query="All" filter="category"/>
      );
}