import React, {Component} from 'react';
import authService from "./api-authorization/AuthorizeService";
import {OrderDetails} from "./OrderDetails";
import Cards from 'react-credit-cards';
import 'react-credit-cards/es/styles-compiled.css'
import {Button, Col, Form, InputGroup, Row} from "reactstrap";
import Input from "reactstrap/es/Input";
import Container from "reactstrap/es/Container";

export class OrderPay extends Component {
  static displayName = OrderPay.name;

  constructor(props) {
    super(props);
    this.state = {
      order: [],
      loading: true,

      cvc: '',
      expiry: '',
      focus: '',
      name: '',
      number: '',
    };
  }

  componentDidMount() {
    this.populateOrder();
  }

  handleInputFocus(e) {
    this.setState({focus: e.target.name});
  }

  handleInputChange(e) {
    const {name, value} = e.target;
    this.setState({[name]: value});
  }

  render() {
    let order = this.state.loading
      ? <p><em>Loading...</em></p>
      : <OrderDetails order={this.state.order} payPage={true}/>;

    let payForm =
      <Container>
        <Row>
          <Col>
            <Cards
              cvc={this.state.cvc}
              expiry={this.state.expiry}
              focused={this.state.focus}
              name={this.state.name}
              number={this.state.number}
            />
          </Col>
          <Col>
            <Form onSubmit={this.submitForm.bind(this)}>
              <InputGroup>
                <Col>
                  <Input type="tel" name="number" placeholder="Card Number"
                         onChange={this.handleInputChange.bind(this)} onFocus={this.handleInputFocus.bind(this)}/>
                </Col>
              </InputGroup>
              <InputGroup>
                <Col>
                  <Input type="text" name="name" placeholder="Name"
                         onChange={this.handleInputChange.bind(this)} onFocus={this.handleInputFocus.bind(this)}/>
                </Col>
              </InputGroup>
              <InputGroup>
                <Col>
                  <Input type="tel" name="expiry" placeholder="Valid Thru"
                         onChange={this.handleInputChange.bind(this)} onFocus={this.handleInputFocus.bind(this)}/>
                </Col>
                <Col>
                  <Input type="tel" name="cvc" placeholder="CVC"
                         onChange={this.handleInputChange.bind(this)} onFocus={this.handleInputFocus.bind(this)}/>
                </Col>
              </InputGroup>
              <InputGroup>
                <Col>
                  <Input type="text" name="address" placeholder="Address"
                         onChange={this.handleInputChange.bind(this)}/>
                </Col>
              </InputGroup>
              <InputGroup>
                <Col>
                  <Button>Pay</Button>
                </Col>
              </InputGroup>
            </Form>
          </Col>
        </Row>
      </Container>;

    return (
      <div>
        <h1>Payment page:</h1>
        <hr/>
        {payForm}
        <hr/>
        {order}
      </div>
    );
  }

  async submitForm(event) {
    event.preventDefault();
    const data = new FormData(event.target);

    const token = await authService.getAccessToken();
    console.log("anus");
    await fetch('api/order/pay' +
      '?orderId=' + this.props.match.params.orderId + 
      '&address=' + data.get('address'), {
      headers: !token ? {} : {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      },
      method: 'POST',
      body: JSON.stringify({
        number: data.get('number'),
        name: data.get('name'),
        expiry: data.get('expiry'),
        cvc: data.get('cvc'),
      }),
    });
    
    this.props.history.push('/orders');
  }

  async populateOrder() {
    const token = await authService.getAccessToken();
    const response = await fetch('api/order/getOrder?orderId=' + this.props.match.params.orderId, {
      headers: !token ? {} : {'Authorization': `Bearer ${token}`}
    });
    const data = (await response.json());
    this.setState({
      order: data,
      loading: false,
    });
  }
}
