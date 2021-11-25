<template>
  <v-dialog
      v-model="dialog"
      class="auth-dialog"
  >
    <template v-slot:activator="{  }">
      <v-btn
          v-bind="$attrs"
          color="primary"
          dark
          @click.stop="dialog = true"
      >
        <v-icon left icon="mdi-account-outline"></v-icon>
        Login
      </v-btn>
    </template>
    <v-card>
      <v-card-title>
        <span class="text-h5">Login</span>
      </v-card-title>
      <v-card-text>
        <v-container>
          <v-row>
            <v-col cols="12">
              <v-text-field
                  label="Email*"
                  required
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                  label="Password*"
                  type="password"
                  required
              ></v-text-field>
            </v-col>
          </v-row>
        </v-container>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn
            flat
            @click="dialog = false"
        >
          Close
        </v-btn>
        <v-btn
            flat
            @click="dialog = false"
        >
          Save
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
  export default {
    name: 'LoginDialog',
    data: () => ({
      dialog: false,
    }),
  }
</script>

<style scoped>

</style>
