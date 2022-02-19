<template>
  <Card class="order-item">
    <template #title>
      <div class="p-d-flex p-jc-between p-ai-center">
        <div class="info">
          <p>#{{ order.id }}</p>
        </div>
        <div class="status" :style="{color:'var(--yellow-300)'}">
          Status: {{order.statusName}}
        </div>
      </div>
      <Timeline :class="{'inActive': isOrderCancelled}" :value="processStatuses" layout="horizontal" align="top">
        <template #marker="slotProps">
          <div
              class="p-timeline-event-marker"
              :class="{'passed': slotProps.item?.code < order.statusCode, 'current': slotProps.item?.code === order.statusCode}"></div>
        </template>
        <template #content="slotProps">
          {{slotProps.item.name}}
        </template>
      </Timeline>
      <Divider/>
    </template>
    <template #content>
      <template v-if="order.orderedProducts && order.orderedProducts.length">
        <div v-for="{product} in order.orderedProducts" :key="product?.id">
          <div v-if="product" class="p-d-flex p-jc-between">
            <span>{{product.name}}</span> -
            <span>{{product.price}}</span>
          </div>
          <Divider/>
        </div>
      </template>
      <div  v-if="!isOrderHandled" class="p-d-flex p-jc-end p-mt-5">
        <Button class="p-button-danger" label="Cancel Order" icon="pi pi-times-circle" @click="handleCancel" />
      </div>
    </template>
  </Card>
</template>

<script>
import Button from 'primevue/button'
import Timeline from 'primevue/timeline'
import ConfirmDialog from 'primevue/confirmdialog'
import {toRefs, ref, onMounted, computed} from 'vue'
import Divider from 'primevue/divider'
import Card from 'primevue/card'
import useOrder from '@/composables/useOrder'
import {useConfirm} from 'primevue/useconfirm'
import {OrderCodes} from '@/Constants'

export default {
  name: 'OrderItem',
  components: {Button, Divider, Card, Timeline, ConfirmDialog},
  props: {
    order: {
      type: Object,
      require: true,
      default: null,
    }
  },
  setup(props) {
    const {orderStatuses, cancelOrder} = useOrder()
    // Filter the cancel status
    const processStatuses = computed(() => orderStatuses.value.filter(status => status.code !== OrderCodes.Cancelled))

    const isOrderCancelled = computed(() => props.order.statusCode === OrderCodes.Cancelled)
    const isOrderHandled = computed(() => props.order.statusCode !== OrderCodes.Ordered)

    const confirm = useConfirm();
    const handleCancel = () => {
      confirm.require({
        message: `Do you want to cancel this order ${props.order.id}?`,
        header: 'Cancel Confirmation',
        icon: 'pi pi-info-circle',
        acceptClass: 'p-button-danger',
        accept: async () => {
          // toast.add({severity:'info', summary:'Confirmed', detail:'Record deleted', life: 3000});
          await cancelOrder(props.order.id)
        },
        reject: () => {
          // Do not thing
        }
      });
    }
    return {
      processStatuses,
      handleCancel,
      isOrderCancelled, isOrderHandled
    }
  },
}
</script>

<style lang="scss">
.order-item {
  .p-timeline.inActive{
    text-decoration: line-through;
    .p-timeline-event-separator {
      .p-timeline-event-marker{
        background: var(--gray-200);
      }
      .p-timeline-event-marker.passed, .p-timeline-event-marker.current {
        background: var(--gray-200);
      }
      .p-timeline-event-marker.passed + .p-timeline-event-connector {
        background: var(--gray-200);
      }
    }
  }

  .p-timeline {
    .p-timeline-event-separator {
      .p-timeline-event-marker{
        background: var(--gray-200);
      }
      .p-timeline-event-marker.passed, .p-timeline-event-marker.current {
        background: var(--yellow-300);
      }
      .p-timeline-event-marker.passed + .p-timeline-event-connector {
        background: var(--yellow-300);
      }
    }
  }
}
</style>
