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
      <div class="p-d-flex p-jc-end p-ai-center">
        <span class="ordered-at">
            Ordered at: {{formatFromNow(order.orderedAt)}}
        </span>
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
      <div  v-if="!isOrderCancelled" class="p-d-flex p-jc-end p-mt-5">
        <Button class="p-button-info" label="Prepare Order" icon="pi pi-history" @click="handlePrepare" />
        <Button class="p-button-success p-ml-5" label="Ready to pick up" icon="pi pi-check-square" @click="handleReady" />
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
import {OrderCodes} from '@/constants'
import {fromNow} from '@/helpers/datetimeHelpers'

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
    const {orderStatuses, updateOrderStatus} = useOrder()
    // Filter the cancel status
    const processStatuses = computed(() => orderStatuses.value.filter(status => status.code !== OrderCodes.Cancelled))

    const isOrderCancelled = computed(() => props.order.statusCode === OrderCodes.Cancelled)

    const confirm = useConfirm();
    const handleReady = () => {
      confirm.require({
        message: `Is the ${props.order.id} ready for client to pick up?`,
        header: 'Done Confirmation',
        icon: 'pi pi-info-circle',
        acceptClass: 'p-button-success',
        accept: async () => {
          await updateOrderStatus(props.order.id, OrderCodes.Ready)
        },
        reject: () => {
          // Do not thing
        }
      });
    }

    const handlePrepare = () => {
      confirm.require({
        message: `Can the Order #${props.order.id} be ready to prepare?`,
        header: 'Prepare Confirmation',
        icon: 'pi pi-info-circle',
        acceptClass: 'p-button-info',
        accept: async () => {
          await updateOrderStatus(props.order.id, OrderCodes.Preparing)
        },
        reject: () => {
          // Do not thing
        }
      });
    }

    const formatFromNow = fromNow

    return {
      processStatuses,
      handleReady,
      handlePrepare,
      isOrderCancelled,
      formatFromNow
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

  .ordered-at {
    font-size: 0.6em;
    color: var(--gray-200);
    font-style: italic;
    font-weight: 200;
  }
}
</style>
