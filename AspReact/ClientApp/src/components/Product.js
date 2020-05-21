import React, {Component} from 'react';
import {
  Button,
  ButtonGroup,
  Card,
  CardBody,
  CardColumns,
  CardImg,
  CardTitle,
  Col,
  Container,
  NavLink,
  Row
} from 'reactstrap';
import CardText from "reactstrap/es/CardText";
import authService from "./api-authorization/AuthorizeService";

export class Product extends Component {
  static displayName = Product.name;

  constructor(props) {
    super(props);
    this.state = {
      isAdded: false,
      count: 0,
      isAuthenticated: false,
      isLoading: true,
    };
  }

  async componentDidMount() {
    this._subscription = authService.subscribe(() => this.populateState());
    await this.populateState();
    console.log(this.state.isAuthenticated);
    if (this.state.isAuthenticated) {
      this.getCartStatus(this.props.item.id);
    }
  }

  componentWillUnmount() {
    authService.unsubscribe(this._subscription);
  }

  async populateState() {
    const [isAuthenticated, user] = await Promise.all([authService.isAuthenticated(), authService.getUser()]);
    this.setState({
      isAuthenticated
    });
  }

  render() {
    return (
      this.props.isCart && this.state.count === 0
        ? null
        :
        <Card>
          <CardImg top width="100%" height="300vw" src={this.props.item.imageSrc} alt="No Image"/>
          <CardBody>
            <CardTitle>{this.props.item.name}</CardTitle>
            <CardText>{this.props.item.description}</CardText>
            <CardText>Price: {this.props.item.price}</CardText>
            {
              this.state.isAuthenticated
                ?
                this.state.isLoading
                  ? <p><em>Loading...</em></p>
                  :
                  this.state.isAdded
                    ?
                    <Row>
                      <Col>
                        <h6>Count: {this.state.count}</h6>
                      </Col>
                      <Col>
                        <ButtonGroup>
                          <Button onClick={() => this.addCartProduct(this.props.item.id)}>+</Button>
                          <Button onClick={() => this.removeCartProduct(this.props.item.id)}>-</Button>
                          <Button onClick={() => this.deleteCartProduct(this.props.item.id)}>Delete</Button>
                        </ButtonGroup>
                      </Col>
                    </Row>
                    : <Button onClick={() => this.addCartProduct(this.props.item.id)}>Add to cart</Button>
                : null
            }
          </CardBody>
        </Card>
    );
  }

  async refreshState(response) {
    console.log(response);
    let cartProduct = response.status === 204
      ? {}
      : !response
        ? {}
        : await response.json();
    if (this.isEmpty(cartProduct.product)) {
      this.setState({
        isAdded: false,
        count: 0,
        isLoading: false,
      });
      if (this.props.isCart) {
        await this.props.deleteEvent(this.props.item.id);
      }
    } else {
      this.setState({
        isAdded: true,
        count: cartProduct.count,
        isLoading: false,
      });
    }
  }

  async getCartStatus(productId) {
    this.setState({
      isLoading: true,
    });
    const token = await authService.getAccessToken();
    const response = await fetch('api/cartProduct/getCartProduct?productId=' + productId, {
      headers: !token ? {} : {'Authorization': `Bearer ${token}`},
    });
    await this.refreshState(response);
  }

  async addCartProduct(productId) {
    this.setState({
      isLoading: true,
    });
    const token = await authService.getAccessToken();
    const response = await fetch('api/cartProduct/Add?productId=' + productId, {
      headers: !token ? {} : {'Authorization': `Bearer ${token}`},
      method: "POST"
    });
    await this.refreshState(response);
  }

  async removeCartProduct(productId) {
    this.setState({
      isLoading: true,
    });
    const token = await authService.getAccessToken();
    const response = await fetch('api/cartProduct/Remove?productId=' + productId, {
      headers: !token ? {} : {'Authorization': `Bearer ${token}`},
      method: "POST"
    });
    await this.refreshState(response);
  }

  async deleteCartProduct(productId) {
    this.setState({
      isLoading: true,
    });
    const token = await authService.getAccessToken();
    const response = await fetch('api/cartProduct/Delete?productId=' + productId, {
      headers: !token ? {} : {'Authorization': `Bearer ${token}`},
      method: "POST"
    });
    await this.refreshState(response);
  }

  isEmpty(obj) {
    for (var key in obj) {
      if (obj.hasOwnProperty(key))
        return false;
    }
    return true;
  }
}
