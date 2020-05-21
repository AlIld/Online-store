import React, {Component} from 'react';
import {Button, Card, CardBody, CardColumns, CardImg, CardTitle, Col, Container, NavLink, Row} from 'reactstrap';
import CardText from "reactstrap/es/CardText";
import {Product} from "./Product";
import {ProductList} from "./ProductList";

export class CategoryProducts extends Component {
  static displayName = CategoryProducts.name;

  constructor(props) {
    super(props);
    this.state = {products: [], loading: true, name:''};
  }

  componentDidMount() {
    this.populateCategoryProducts();
  }

  componentWillReceiveProps(nextProps, nextContext) {
    setTimeout(this.populateCategoryProducts.bind(this))
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      :
      <div>
        <h1>Products for category {this.state.name}:</h1>
        <hr/>
        <ProductList products={this.state.products}/>
      </div>;

    return (
      <div>
        {contents}
      </div>
    );
  }

  async populateCategoryProducts() {
    const response = await fetch('api/product/ProductsForCategory?categoryId=' + this.props.match.params.categoryId);
    const data = await response.json();
    this.setState({
      products: data,
      loading: false
    });
    
    await this.populateCategoryName();
  }
  
  async populateCategoryName(){
    const response = await fetch('api/category/getCategory?categoryId=' + this.props.match.params.categoryId);
    const data = await response.json();
    this.setState({
      name: data.name,
    });
  } 
}
