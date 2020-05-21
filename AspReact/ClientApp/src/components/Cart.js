import React, {Component} from 'react';
import {Button, Card, CardBody, CardColumns, CardImg, CardTitle, Col, Container, NavLink, Row} from 'reactstrap';
import CardText from "reactstrap/es/CardText";
import authService from "./api-authorization/AuthorizeService";
import {Product} from "./Product";
import {ProductList} from "./ProductList";
import {Link, Redirect} from "react-router-dom";

export class Cart extends Component {
  static displayName = Cart.name;

  constructor(props) {
    super(props);
    this.state = {
      products: [],
      productIds: [],
      loading: true
    };
  }

  componentDidMount() {
    this.populateCartProducts();
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      :
      <ProductList deleteEvent={async (productId) => {
        await this.delete(productId)
      }} products={this.state.cartProducts.map(x => x.product)} isCart={true}/>;
  
    return (
      <div>
        {
          this.state.productIds.length > 0
            ? <Button onClick={async () => {
              await this.makeOrder()
            }}>Make an order</Button>
            : null
        }
        <h1>Products in your cart:</h1>
        <hr/>
        {contents}
      </div>
    );
  }

  async makeOrder() {
    const token = await authService.getAccessToken();
    const response = await fetch('api/order/makeOrder', {
      headers: !token ? {} : {'Authorization': `Bearer ${token}`},
      method: "POST"
    });
    if (response.ok) {
      this.props.history.push('/orders');
    }
  }

  async delete(productId) {
    let arr = this.state.productIds;
    let index = arr.indexOf(productId);
    if (index > -1) {
      arr.splice(index, 1)
    }

    this.setState({
      productIds: arr
    });
  }

  async populateCartProducts() {
    const token = await authService.getAccessToken();
    const response = await fetch('api/cartProduct/Index', {
      headers: !token ? {} : {'Authorization': `Bearer ${token}`}
    });
    const data = (await response.json());
    this.setState({
      cartProducts: data,
      loading: false,
      productIds: data.map(x => x.product.id),
    });
  }
}
