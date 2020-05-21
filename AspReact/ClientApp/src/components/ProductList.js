import React, {Component} from 'react';
import {Button, Card, CardBody, CardColumns, CardImg, CardTitle, Col, Container, NavLink, Row} from 'reactstrap';
import CardText from "reactstrap/es/CardText";
import authService from "./api-authorization/AuthorizeService";
import {Product} from "./Product";

export class ProductList extends Component {
  static displayName = ProductList.name;

  render() {
    let products = this.props.products.map(item => <Product deleteEvent={this.props.deleteEvent} item={item} isCart={this.props.isCart}/>);
    
    return(
      <CardColumns>{products}</CardColumns>
    );
  }
}
