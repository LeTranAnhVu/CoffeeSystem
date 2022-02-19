<template>
  <div>
    <div class="p-d-flex p-jc-between p-mb-2">
      <Skeleton width="10rem"></Skeleton>
      <Skeleton width="10rem"></Skeleton>
    </div>
    <Skeleton class="p-mb-2"></Skeleton>
    <Skeleton width="10rem" class="p-mb-2"></Skeleton>
  </div>
</template>

<script>
import Skeleton from 'primevue/skeleton'

export default {
  name: 'OrderItemSkeleton',
  components: {
    Skeleton
  }
}
</script>

<style scoped>

</style>
