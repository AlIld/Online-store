import React, {Component} from 'react';
import {Button, Card, CardBody, CardColumns, CardImg, CardTitle, Col, Container, NavLink, Row, Table} from 'reactstrap';

export class OrderDetails extends Component {
  static displayName = OrderDetails.name;

  constructor(props) {
    super(props);
  }

  render() {
    let contents =
      <tr>
        <th scope="row">{this.props.order.id}</th>
        <td>{new Date(this.props.order.dateTime).toLocaleString()}</td>
        <td>{this.props.order.fullPrice}</td>
        {
          this.props.payPage
            ? null
            :
            <td>{this.props.order.isDelivered ? 'Delivered' : 'Not delivered'}</td>
        }
        {
          this.props.payPage
            ? null
            :
            <td>
              {
                this.props.order.isPaid
                  ? 'Paid'
                  : <Button onClick={() => this.props.history.push('/orders/pay/' + this.props.order.id)}>Pay</Button>
              }
            </td>
        }
      </tr>;

    let orderProducts = this.props.order.orderProducts.map(x =>
      <tr>
        <th scope="row">{x.product.id}</th>
        <td>{x.count}</td>
        <td>{x.product.name}</td>
        <td>{x.product.price}</td>
      </tr>);

    return (
      <div>

        <h1>Order info:</h1>
        <Table>
          <thead>
          <tr>
            <th>#</th>
            <th>Date and time</th>
            <th>Full Price</th>
            {
              this.props.payPage
                ? null
                :
                <th>Delivered</th>
            }
            {
              this.props.payPage
                ? null
                :
                  <th>Paid</th>
            }
          </tr>
          </thead>
          <tbody>
          {contents}
          </tbody>
        </Table>

        <hr/>
        <h1>Order's products info:</h1>
        <Table>
          <thead>
          <tr>
            <th>#</th>
            <th>Count</th>
            <th>Name</th>
            <th>Price</th>
          </tr>
          </thead>
          <tbody>
          {orderProducts}
          </tbody>
        </Table>

      </div>

    );
  }
}